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
        public TVGuide()
        {
            // download tv listings for today
            var data = getChannels();
        }

        public List<TVGuideChannel> getChannels()
        {
            List<TVGuideChannel> result = new List<TVGuideChannel>();
            string apiUrl = String.Format(
                "http://api.tvmaze.com/schedule?country=GB&date={0}",
                DateTime.Now.ToString("yyyy-MM-dd"));


            var programs = getJSON(apiUrl);
            string test = "";
            foreach (Dictionary<string, object> program in programs)
            {
                var show = program["show"] as Dictionary<string, object>;
                var network = show["network"] as Dictionary<string, object>;
                test += network["name"] as string + "\n";


            }
            MessageBox.Show(test);
            return result; 
        }

        /**
         * Utility function to convert json from a http end point to a nested series of dictionaries
         */
        private List<object> getJSON(string apiUrl)
        {
            try
            {
                // fetch tv listings json
                var request = WebRequest.Create(apiUrl) as HttpWebRequest;
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
                            // convert JSON object to a series of nested dictionaries

                            Func<string, List<object>> parseJSONArr = null;
                            Func<string, Dictionary<string, object>> parseJSONObj = null;

                            parseJSONArr = (string json) =>
                            {
                                var values = JsonConvert.DeserializeObject<List<object>>(json);
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
                            parseJSONObj = (string json) =>
                            {
                                var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
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
                            string responseText = reader.ReadToEnd().Trim();
                            if (responseText[0] == '[')
                            {
                                // return list of objects
                                return parseJSONArr(responseText);
                            }
                            else
                            {
                                // return list with single object
                                List<object> result = new List<object>();
                                result.Add(parseJSONObj(responseText));
                                return result;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Could not get TV listings",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
