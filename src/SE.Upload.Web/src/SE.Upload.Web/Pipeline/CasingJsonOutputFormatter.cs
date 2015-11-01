// ==========================================================================
// CasingJsonOutputFormatter.cs
// Green Parrot Framework
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ========================================================================== 

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Formatters;
using Microsoft.Framework.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SE.Upload.Web.Pipeline
{
    public sealed class CasingJsonOutputFormatter : JsonOutputFormatter
    {
        private readonly JsonSerializerSettings camelCaseSettings = new JsonSerializerSettings();

        public JsonSerializerSettings CamelCaseSettings
        {
            get
            {
                return camelCaseSettings;
            }
        }

        public CasingJsonOutputFormatter()
        {
            camelCaseSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public override Task WriteResponseBodyAsync(OutputFormatterContext context)
        {
            HttpResponse response = context.HttpContext.Response;
            HttpRequest request = context.HttpContext.Request;

            StringValues casingValues;

            bool camelCase = request.Headers.TryGetValue("X-Casing", out casingValues) && casingValues.Contains("camelCase", StringComparer.OrdinalIgnoreCase);

            using (HttpResponseStreamWriter httpResponseStreamWriter = new HttpResponseStreamWriter(response.Body, context.SelectedEncoding))
            {
                WriteObject(camelCase, httpResponseStreamWriter, context.Object);
            }

            return Task.FromResult(true);
        }

        private void WriteObject(bool camelCase, TextWriter writer, object value)
        {
            using (JsonWriter jsonWriter = CreateJsonWriter(writer))
            {
                CreateJsonSerializer(camelCase).Serialize(jsonWriter, value);
            }
        }

        private JsonSerializer CreateJsonSerializer(bool camelCase)
        {
            JsonSerializer serializer = JsonSerializer.Create(camelCase ? CamelCaseSettings : SerializerSettings);

            return serializer;
        }
    }
}
