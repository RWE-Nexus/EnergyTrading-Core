namespace EnergyTrading.Deployment.Confluence
{
    using System;
    using System.IO;
    using System.Text;

    public class ConfluenceClient
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Url { get; set; }

        public ConfluenceClient(string userName, string password, string url = "http://confluence.rwe.com:8081/rpc/soap-axis/confluenceservice-v1")
        {
            if (userName == null) { throw new ArgumentNullException("userName"); }
            if (password == null) { throw new ArgumentNullException("password"); }
            this.UserName = userName;
            this.Password = password;
            this.Url = url;
        }

        public RemotePage CreatePage(string spaceIdentifier, long parentPageId, string pageTitle, string pageContent)
        {
            RemotePage remotePage = null;
            this.ExecuteLoggedIn(
                (confluence, token) =>
                {
                    var newPage = new RemotePage
                    {
                        content = pageContent,
                        parentId = parentPageId,
                        space = spaceIdentifier,
                        creator = this.UserName,
                        title = pageTitle
                    };

                    var storedPage = confluence.storePage(token, newPage);
                    remotePage = storedPage;
                });

            return remotePage;
        }

        public RemotePage AppendContentToPage(long pageId, string pageContent)
        {
            RemotePage remotePage = null;
            this.ExecuteLoggedIn(
                (confluence, token) =>
                {
                    var page = confluence.getPage(token, pageId);
                    page.content += pageContent;
                    var options = new RemotePageUpdateOptions { versionComment = "Updated as a result of publishing package version and sending notification email" };
                    var updatedPage = confluence.updatePage(token, page, options);
                    remotePage = updatedPage;
                });

            return remotePage;
        }

        public RemoteComment AddCommentToPage(long pageId, string commentTitle, string commentContent)
        {
            RemoteComment comment = null;
            this.ExecuteLoggedIn(
                (confluence, token) =>
                {
                    var remoteComment = new RemoteComment { pageId = pageId, creator = this.UserName, title = commentTitle, content = commentContent, };
                    var addedComment = confluence.addComment(token, remoteComment);
                    comment = addedComment;
                });

            return comment;
        }

        public void AttachFileToPage(long pageId, string title, string fileName, string filePath, string fileContentType, string attachedFileName = null)
        {
            this.ExecuteLoggedIn(
                (confluence, token) =>
                {
                    var file = new FileInfo(Path.Combine(filePath, fileName));
                    using (var fileStream = file.OpenRead())
                    using (var reader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        var str = reader.ReadToEnd();
                        var attachmentData = new byte[str.Length * sizeof(char)];
                        Buffer.BlockCopy(str.ToCharArray(), 0, attachmentData, 0, attachmentData.Length);
                        if (attachmentData.Length > 0)
                        {
                            var attachment = new RemoteAttachment
                            {
                                pageId = pageId,
                                title = title,
                                fileName = attachedFileName ?? fileName,
                                contentType = fileContentType,
                                created = DateTime.Now,
                                creator = this.UserName,
                                fileSize = attachmentData.Length
                            };
                            confluence.addAttachment(token, attachment, attachmentData);
                        }
                    }
                });
        }

        public void DownloadAttachmentsFromPage(long pageId, string downloadPath)
        {
            this.ExecuteLoggedIn(
                (confluence, token) =>
                {
                    var attachments = confluence.getAttachments(token, pageId);
                    foreach (var remoteAttachment in attachments)
                    {
                        var data = confluence.getAttachmentData(token, pageId, remoteAttachment.fileName, 0);
                        var filePath = Path.Combine(downloadPath, remoteAttachment.fileName);
                        File.WriteAllBytes(filePath, data);
                    }
                });
        }

        private void ExecuteLoggedIn(Action<ConfluenceSoapServiceService, string> code)
        {
            ConfluenceSoapServiceService confluence = null;
            var token = string.Empty;
            try
            {
                confluence = new ConfluenceSoapServiceService
                {
                    Url = this.Url
                };
                token = confluence.login(this.UserName, this.Password);

                code(confluence, token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (confluence != null)
                {
                    confluence.logout(token);
                }
            }
        }
    }
}
