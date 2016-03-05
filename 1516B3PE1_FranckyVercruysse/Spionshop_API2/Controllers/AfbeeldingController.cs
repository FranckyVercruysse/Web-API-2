using Spionshop_API2.Models.dto;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;

//http://www.dotnetcurry.com/aspnet/1120/aspnet-webapi-binary-contents-images

namespace Spionshop_API2.Controllers
{
    [EnableCors("*", "*", "*")]
    public class AfbeeldingController : ApiController
    {
        [Route("LijstAfbeeldingen")]
        public List<FilesInfoDTO> GetFiles()
        {
            List<FilesInfoDTO> files = new List<FilesInfoDTO>();

            var padImages = HostingEnvironment.MapPath("~/Resources/Images");


            foreach (var item in Directory.GetFiles(padImages))
            {
                FileInfo f = new FileInfo(item);
                files.Add(new FilesInfoDTO() { FileName = f.Name });
            }
            return files;
        }

        /// <summary>
        /// Return the image as Byte Array through the HttpResponseMessage object  
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        [Route("BytesArray/{fileName}/{ext}/{pad}")]
        public HttpResponseMessage Get(string fileName, string ext, string pad)
        {
            if (pad.ToLower() == "resources") pad = "~/Resources/Images";
            if (pad.ToLower() == "images") pad = "~/Images/";

            var padImages = HostingEnvironment.MapPath(pad);
            //S1: Construct File Path
            var filePath = Path.Combine(padImages, fileName + "." + ext);
            if (!File.Exists(filePath)) //Not found then throw Exception ////throw new HttpResponseException(HttpStatusCode.NotFound);
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            HttpResponseMessage Response = new HttpResponseMessage(HttpStatusCode.OK);

            //S2:Read File as Byte Array
            byte[] fileData = File.ReadAllBytes(filePath);

            if (fileData == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            //S3:Set Response contents and MediaTypeHeaderValue
            Response.Content = new ByteArrayContent(fileData);
            Response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return Response;
        }


        [Route("CopyAfbeelding/{fileName}/{id}")]
        public FilesInfoDTO GetCopyFiles(string fileName, short id)
        {
            string oldPathAndName, newPathAndName;

            oldPathAndName = Path.Combine(HostingEnvironment.MapPath("~/Resources/Images"), fileName);
            newPathAndName = Path.Combine(HostingEnvironment.MapPath("~/Images/"), id.ToString() + ".gif");     // id = artikel_id => afbeelding behoort tot een artikel

            System.IO.File.Copy(oldPathAndName, newPathAndName, true);      // copiëren van de grote afbeeldingen

            oldPathAndName = Path.Combine(HostingEnvironment.MapPath("~/Resources/Images/thumbs"), "Thumbs" + fileName);
            newPathAndName = Path.Combine(HostingEnvironment.MapPath("~/Images/thumbs"), id.ToString() + ".gif");   // id = artikel_id => afbeelding behoort tot een artikel

            System.IO.File.Copy(oldPathAndName, newPathAndName, true);       // copiëren van de kleine (thumbnails) afbeeldingen

            FilesInfoDTO filesInfoDTO = new FilesInfoDTO();
            filesInfoDTO.Artikel_id = id;
            filesInfoDTO.FileName = fileName;
            return filesInfoDTO;
        }

        [Route("DeleteAfbeelding/{id}")]
        public HttpResponseMessage GetDeleteFile(short id)                      // delete gebeurt met een http get request (benaming speelt hier geen rol)
        {
            string PathAndName;

            PathAndName = Path.Combine(HostingEnvironment.MapPath("~/Images/"), id.ToString() + ".gif");
            if (!File.Exists(PathAndName))
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            System.IO.File.Delete(PathAndName);

            PathAndName = Path.Combine(HostingEnvironment.MapPath("~/Images/thumbs"), id.ToString() + ".gif");
            if (!File.Exists(PathAndName))
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            System.IO.File.Delete(PathAndName);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
