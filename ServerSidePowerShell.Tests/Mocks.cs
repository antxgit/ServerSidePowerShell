using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace ServerSidePowerShell.Tests
{

    public class WebMocks
    {
        internal string PhysicalPath;

        public static Encoding Encoding { get; set; } = Encoding.UTF8;
        private HttpResponseBaseImpl HttpResponse;

        public WebMocks()
        {
            
        }

        public HttpContextBase CreateHttpContext()
        {
            var serverVariables = new NameValueCollection {
                { "SomeServerIPAddress", "127.0.0.1" },
                { "AnotherAppVariable", "Unit Test Value" }
            };

            var httpRequest = new Mock<HttpRequestBase>(MockBehavior.Loose);
            httpRequest.Setup(x => x.ServerVariables.Get(It.IsAny<string>()))
                .Returns<string>(x =>
                {
                    return serverVariables[x];
                });

            httpRequest.SetupGet(x => x.PhysicalPath).Returns(PhysicalPath);

            HttpResponse = new HttpResponseBaseImpl();


            var httpContext = (new Mock<HttpContextBase>(MockBehavior.Loose));
            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);
            httpContext.Setup(x => x.Response).Returns(HttpResponse);

            return httpContext.Object;
        }

        internal string GetResponseAsText()
        {
            Stream stream = HttpResponse.Stream;
            stream.Seek(0, SeekOrigin.Begin);

            StreamReader streamReader = new StreamReader(stream);
            return streamReader.ReadToEnd();
        }
    }

    public class HttpResponseBaseImpl : HttpResponseBase
    {
        public Stream Stream { get; } = new MemoryStream();

        public override void Write(char ch)
        {
            System.Diagnostics.Debug.WriteLine("Write");
            Write(new char[ch], 0, 1);
        }
        public override void Write(char[] buffer, int index, int count)
        {
            System.Diagnostics.Debug.WriteLine("Write");
            BinaryWrite(WebMocks.Encoding.GetBytes(buffer));
        }
        public override void Write(object obj)
        {
            throw new NotImplementedException();
        }
        public override void Write(string s)
        {
            System.Diagnostics.Debug.WriteLine("Write");
            BinaryWrite(WebMocks.Encoding.GetBytes(s));
        }
        public override void BinaryWrite(byte[] buffer)
        {
            System.Diagnostics.Debug.WriteLine("Write");
            Stream.Write(buffer, 0, buffer.Length);
        }
    }

}
