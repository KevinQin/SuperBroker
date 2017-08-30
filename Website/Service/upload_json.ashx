<%@ webhandler Language="C#" class="Upload" %>

using System;
using System.Collections;
using System.Web;
using System.IO;
using System.Globalization;
using LitJson;

public class Upload : IHttpHandler
{
	private HttpContext context;

	public void ProcessRequest(HttpContext context)
	{
        String aspxUrl = context.Request.Path.Substring(0, context.Request.Path.IndexOf("/") + 1);
		
		//文件保存目录路径
		String savePath = "/attached/";
		//文件保存目录URL
		String saveUrl = aspxUrl + savePath;		
		//最大文件大小
		int maxSize = 30000000;
		this.context = context;

		HttpPostedFile imgFile = context.Request.Files[0];		
		String dirPath = context.Server.MapPath(savePath);		
		String dirName = context.Request.QueryString["dir"];
		if (String.IsNullOrEmpty(dirName)) {
			dirName = "image";
		}
		String fileName = imgFile.FileName;
		String fileExt = Path.GetExtension(fileName).ToLower();
		if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
		{
			showError("上传文件大小超过限制。");
		}
		
		//创建文件夹
		dirPath += dirName + "/";
		saveUrl += dirName + "/";
		if (!Directory.Exists(dirPath)) {
			Directory.CreateDirectory(dirPath);
		}
		String ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
		dirPath += ymd + "/";
		saveUrl += ymd + "/";
		if (!Directory.Exists(dirPath)) {
			Directory.CreateDirectory(dirPath);
		}
        String fileUrl = "";
        Hashtable hash = new Hashtable();
        try
        {
            String newFileName = DateTime.Now.ToString("HHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            String filePath = dirPath + newFileName;
            imgFile.SaveAs(filePath);
            fileUrl = saveUrl + newFileName;           
            hash["error"] = "";
        }
        catch(Exception ex) {
            hash["error"] = ex.Message;            
        }
        hash["url"] = fileUrl.Replace("//","/");
        hash["filename"] = fileName;
		context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
		context.Response.Write(JsonMapper.ToJson(hash));
		context.Response.End();
	}

	private void showError(string message)
	{
		Hashtable hash = new Hashtable();
		hash["error"] = 1;
		hash["message"] = message;
		context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
		context.Response.Write(JsonMapper.ToJson(hash));
		context.Response.End();
	}

	public bool IsReusable
	{
		get
		{
			return true;
		}
	}
}
