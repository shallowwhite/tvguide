using System;
using System.Net;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json;

namespace TVGuide.App
{
    class TVGuide
    {
        private string authenticationToken = "";

        public TVGuide()
        {
            // download tv listings for today
            var data = getAuthenticationToken();
        }

        public List<TVGuideChannel> getChannels()
        {
            List<TVGuideChannel> result = new List<TVGuideChannel>();

            /*var programs = getJSON(apiUrl);
            string test = "";
            foreach (Dictionary<string, object> program in programs)
            {
                var show = program["show"] as Dictionary<string, object>;
                var network = show["network"] as Dictionary<string, object>;
                test += network["name"] as string + "\n";


            }
            MessageBox.Show(test);*/
            return result; 
        }

        private string getTVData()
        {
            // download xmltv file
            // convert to json

        }

        /**
         * requestJSON()
         */
        private List<object> requestJSON(string url, string format = "application/json", string post = null)
        {
            try
            {
                // fetch tv listings json
                var request = WebRequest.Create(url) as HttpWebRequest;
                request.ContentType = "application/json";
                
                if (jsonPost != null)
                {
                    request.Method = "POST";
                    using (var writer = new StreamWriter(request.GetRequestStream()))
                    {
                        writer.Write(jsonPost);
                    }
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    // check http request was successful
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).",
                            response.StatusCode,
                            response.StatusDescription));
                    }
                    else
                    {
                        // read all data from http request
                        using (var reader = new StreamReader(
                            response.GetResponseStream(), System.Text.Encoding.UTF8))
                        {
                            return convertJSON(reader.ReadToEnd());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /**
         * convertJSON()
         * Utility function to convert json from a url to nested series of
         * Lists and Dictionaries
         */
        private List<Object> convertJSON(string json)
        {
            // convert JSON object to a series of nested dictionaries
            Func<string, List<object>> parseJSONArr = null;
            Func<string, Dictionary<string, object>> parseJSONObj = null;

            parseJSONArr = (string jsonString) =>
            {
                var values = JsonConvert.DeserializeObject<List<object>>(jsonString);
                var result = new List<object>();

                foreach (object it in values)
                {
                    if (it is Newtonsoft.Json.Linq.JObject)
                    {
                        result.Add(parseJSONObj(it.ToString()));
                    }
                    else if (it is Newtonsoft.Json.Linq.JArray)
                    {
                        result.Add(parseJSONArr(it.ToString()));
                    }
                    else
                    {
                        result.Add(it);
                    }
                }

                return result;
            };
            parseJSONObj = (string jsonString) =>
            {
                var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
                var result = new Dictionary<string, object>();

                foreach (KeyValuePair<string, object> it in values)
                {
                    if (it.Value is Newtonsoft.Json.Linq.JObject)
                    {
                        result.Add(it.Key, parseJSONObj(it.Value.ToString()));
                    }
                    else if (it.Value is Newtonsoft.Json.Linq.JArray)
                    {
                        result.Add(it.Key, parseJSONArr(it.Value.ToString()));
                    }
                    else
                    {
                        result.Add(it.Key, it.Value);
                    }
                }
                return result;
            };
            json = json.Trim();
            if (json[0] == '[') // check if root element is an array
            {
                // return list of objects
                return parseJSONArr(json);
            }
            else
            {
                // return list with single object
                List<object> result = new List<object>();
                result.Add(parseJSONObj(json));
                return result;
            }
        }
    }
}
