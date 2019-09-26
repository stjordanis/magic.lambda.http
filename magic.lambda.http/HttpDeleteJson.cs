﻿/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using System.Collections.Generic;
using magic.node;
using magic.http.contracts;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.http
{
    [Slot(Name = "http.delete.json")]
    public class HttpDeleteJson : ISlot
    {
        readonly IHttpClient _httpClient;

        public HttpDeleteJson(IHttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public void Signal(ISignaler signaler, Node input)
        {
            if (input.Children.Count() > 1 || input.Children.Any(x => x.Name != "token"))
                throw new ApplicationException("[http.delete.json] can only handle one [token] child node");

            var url = input.GetEx<string>();
            var token = input.Children.FirstOrDefault(x => x.Name == "token")?.GetEx<string>();

            // Notice, to sanity check the result we still want to roundtrip through a JToken result.
            input.Value = _httpClient.DeleteAsync<string>(url, token).Result;
            input.Clear();
        }
    }
}
