using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace MapLab.Common.Attributes.Validation
{
    public class GeoJsonAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is IFormFile file)
            {
                try
                {
                    using var stream = file.OpenReadStream();
                    using var reader = new StreamReader(stream);
                    var jsonString = reader.ReadToEnd();
                    var json = JObject.Parse(jsonString);

                    if (!json.ContainsKey("type") || json["type"] == null)
                    {
                        return false;
                    }

                    var type = json["type"].ToString();
                    string[] validTypes = { "Point", "MultiPoint", "LineString", "MultiLineString",
                                        "Polygon", "MultiPolygon", "GeometryCollection", "Feature", "FeatureCollection" };

                    if (!Array.Exists(validTypes, t => t.Equals(type, StringComparison.OrdinalIgnoreCase)))
                    {
                        return false;
                    }

                    if ((type == "FeatureCollection" && !json.ContainsKey("features")) ||
                        (type == "Feature" && !json.ContainsKey("geometry")) ||
                        ((type.Contains("Point") || type.Contains("Polygon") || type.Contains("LineString")) && !json.ContainsKey("coordinates")))
                    {
                        return false;
                    }

                    return true;
                }
                catch (JsonReaderException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }
    }
}
