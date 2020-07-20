using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;

namespace DocuSign_Digital_Signature_API.Controllers
{
    public class DigitalSignatureController : ApiController
    {
		private const string accessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6IjY4MTg1ZmYxLTRlNTEtNGNlOS1hZjFjLTY4OTgxMjIwMzMxNyJ9.eyJUb2tlblR5cGUiOjUsIklzc3VlSW5zdGFudCI6MTU5NTIxNjc0MSwiZXhwIjoxNTk1MjQ1NTQxLCJVc2VySWQiOiIyN2I1MGNiNC01MzljLTQ2YTctOTgwMS03ODVhMDY1ZjY3NDgiLCJzaXRlaWQiOjEsInNjcCI6WyJzaWduYXR1cmUiLCJjbGljay5tYW5hZ2UiLCJvcmdhbml6YXRpb25fcmVhZCIsInJvb21fZm9ybXMiLCJncm91cF9yZWFkIiwicGVybWlzc2lvbl9yZWFkIiwidXNlcl9yZWFkIiwidXNlcl93cml0ZSIsImFjY291bnRfcmVhZCIsImRvbWFpbl9yZWFkIiwiaWRlbnRpdHlfcHJvdmlkZXJfcmVhZCIsImR0ci5yb29tcy5yZWFkIiwiZHRyLnJvb21zLndyaXRlIiwiZHRyLmRvY3VtZW50cy5yZWFkIiwiZHRyLmRvY3VtZW50cy53cml0ZSIsImR0ci5wcm9maWxlLnJlYWQiLCJkdHIucHJvZmlsZS53cml0ZSIsImR0ci5jb21wYW55LnJlYWQiLCJkdHIuY29tcGFueS53cml0ZSJdLCJhdWQiOiJmMGYyN2YwZS04NTdkLTRhNzEtYTRkYS0zMmNlY2FlM2E5NzgiLCJhenAiOiJmMGYyN2YwZS04NTdkLTRhNzEtYTRkYS0zMmNlY2FlM2E5NzgiLCJpc3MiOiJodHRwczovL2FjY291bnQtZC5kb2N1c2lnbi5jb20vIiwic3ViIjoiMjdiNTBjYjQtNTM5Yy00NmE3LTk4MDEtNzg1YTA2NWY2NzQ4IiwiYW1yIjpbImludGVyYWN0aXZlIl0sImF1dGhfdGltZSI6MTU5NTIxNjczOSwicHdpZCI6IjIwZTQyMzc3LWJjMTgtNDUxOS04NDkxLTY3ODUyYzNhMmZiYSJ9.memoWamquAHWJE8IVLbnhrCP8eEzc6nw3rUHbk5zA3GRcQL5XC1Jxm8tNMdJokqkwx13qHI9D59iKGSLz_SlPSBcEDU1PEaUVT3K91MxU-Xh8XtZejFy9VsLf4Dd_2rCMaQu1kTfrQwjILQ3RODvpC3Cwiv2749PUCxap-2KXNQvakdrfG7KVZJZNYG2mw_MIPKOV2que3QuK2ACXYegP5_0qIy9zXJCXV9l7QOfJeepSUFdM0DtGN5-dZsJ53rqZtHh_72uJS3W-VvZd1SO83z7lzlJbl_QCinV8mbQCx7rGZbJmc6UM_JdQRsImv3oTBpxHre8DKzc1srPtnXLAQ";
		private const string accountId = "10983993";
		private const string signerName = "Jithin Jayan";
		private const string signerEmail = "jithin@resemblesystems.com";
		private const string docName = "World_Wide_Corp_lorem.pdf";
		private const string signerClientId = "1000";

		// Additional constants
		private const string basePath = "https://demo.docusign.net/restapi";
		private const string returnUrl = "https://localhost:44377/help";

		[HttpGet]
		public async Task<DocuSign.eSign.Model.ViewUrl> sendenvelope( )
		{
			Document document = new Document
			{
				DocumentBase64 = Convert.ToBase64String(ReadContent(docName)),
				Name = "Lorem Ipsum",
				FileExtension = "pdf",
				DocumentId = "1"
			};
			Document[] documents = new Document[] { document };

			// Create the signer recipient object 
			Signer signer = new Signer
			{
				Email = signerEmail,
				Name = signerName,
				ClientUserId = signerClientId,
				RecipientId = "1",
				RoutingOrder = "1"
				
			};

			// Create the sign here tab (signing field on the document)
			SignHere signHereTab = new SignHere
			{
				DocumentId = "1",
				PageNumber = "1",
				RecipientId = "1",
				TabLabel = "Sign Here Tab",
				XPosition = "195",
				YPosition = "147"
			};
			SignHere[] signHereTabs = new SignHere[] { signHereTab };

			// Add the sign here tab array to the signer object.
			signer.Tabs = new Tabs { SignHereTabs = new List<SignHere>(signHereTabs) };
			// Create array of signer objects
			Signer[] signers = new Signer[] { signer };
			// Create recipients object
			Recipients recipients = new Recipients { Signers = new List<Signer>(signers) };
			// Bring the objects together in the EnvelopeDefinition
			EnvelopeDefinition envelopeDefinition = new EnvelopeDefinition
			{
				EmailSubject = "ITFC Memo Approval",
				Documents = new List<Document>(documents),
				Recipients = recipients,
				Status = "sent"
			};

			// 2. Use the SDK to create and send the envelope
			ApiClient apiClient = new ApiClient(basePath);
			apiClient.Configuration.AddDefaultHeader("Authorization", "Bearer " + accessToken);
			EnvelopesApi envelopesApi = new EnvelopesApi(apiClient.Configuration);
			EnvelopeSummary results =  envelopesApi.CreateEnvelope(accountId, envelopeDefinition);
            //ViewData["results"] = $"Envelope status: {results.Status}. Envelope ID: {results.EnvelopeId}";

            string envelopeId =  results.EnvelopeId;
            RecipientViewRequest viewOptions = new RecipientViewRequest
            {
                ReturnUrl = returnUrl,
                ClientUserId = signerClientId,
                AuthenticationMethod = "none",
                UserName = signerName,
                Email = signerEmail
            };

			// Use the SDK to obtain a Recipient View URL
			ViewUrl viewUrl =  envelopesApi.CreateRecipientView(accountId, envelopeId, viewOptions);

			return viewUrl;
		}


		internal static byte[] ReadContent(string fileName)
		{
			byte[] buff = null;
			string directory = Directory.GetCurrentDirectory();
			string path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", fileName);
			//string path = "C:\\Resources";
			using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader br = new BinaryReader(stream))
				{
					long numBytes = new FileInfo(path).Length;
					buff = br.ReadBytes((int)numBytes);
				}
			}

			return buff;
		}
	}
}
