using DriverApi;
using ScadaCore.Tags.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace ScadaCore.Configuration
{
    // TODO: Add save on close
    public class ScadaConfiguration
    {
        private readonly XDocument _xml;
        private readonly IDriver _simulationDriver;
        private readonly IDriver _rtuDriver;

        public ScadaConfiguration(string configPath, IDriver simulationDriver, IDriver rtuDriver)
        {
            _xml = XDocument.Load(configPath);
            _simulationDriver = simulationDriver;
            _rtuDriver = rtuDriver;
        }

        public ICollection<Tag> GetTags()
        {
            var tags = _xml.Element("tags").Elements();
            var tagList = new List<Tag>();

            foreach (var tag in tags)
            {
                switch (tag.Name.LocalName)
                {
                    case "di":
                        tagList.Add(new DigitalInputTag(
                                tag.Attribute("id").Value,
                                tag.Attribute("description").Value,
                                tag.Attribute("ioAddress").Value,
                                Convert.ToDouble(tag.Value),
                                Convert.ToInt32(tag.Attribute("scanTime").Value),
                                Convert.ToBoolean(tag.Attribute("isScanOn").Value),
                                tag.Attribute("driver").Value == "simulation" ? _simulationDriver : _rtuDriver
                            ));
                        break;
                    case "do":
                        tagList.Add(new DigitalOutputTag(
                                tag.Attribute("id").Value,
                                tag.Attribute("description").Value,
                                tag.Attribute("ioAddress").Value,
                                Convert.ToDouble(tag.Value),
                                Convert.ToDouble(tag.Attribute("initialValue").Value)
                            ));
                        break;
                    case "ai":

                        AnalogInputTag analogInputTag = new AnalogInputTag(
                            tag.Attribute("id").Value,
                            tag.Attribute("description").Value,
                            tag.Attribute("ioAddress").Value,
                            Convert.ToDouble(tag.Nodes().OfType<XText>().First().ToString()),
                            Convert.ToInt32(tag.Attribute("scanTime").Value),
                            Convert.ToBoolean(tag.Attribute("isScanOn").Value),
                            Convert.ToDouble(tag.Attribute("lowLimit").Value),
                            Convert.ToDouble(tag.Attribute("highLimit").Value),
                            (Unit)Enum.Parse(typeof(Unit), tag.Attribute("unit").Value),
                            tag.Attribute("driver").Value == "simulation" ? _simulationDriver : _rtuDriver,
                            null
                        );

                        var alarms = tag.Descendants("alarm")
                            .Select(a => new Alarm { TagName = a.Attribute("id").Value, Priority = Convert.ToInt32(a.Attribute("priority").Value), Threshold = Convert.ToDouble(a.Attribute("threshold").Value), Type = a.Attribute("type").Value, Unit = analogInputTag.Unit });

                        analogInputTag.Alarms = alarms.ToList();

                        tagList.Add(analogInputTag);
                        break;
                    case "ao":
                        tagList.Add(new AnalogOutputTag(
                                tag.Attribute("id").Value,
                                tag.Attribute("description").Value,
                                tag.Attribute("ioAddress").Value,
                                Convert.ToDouble(tag.Value),
                                Convert.ToDouble(tag.Attribute("initialValue").Value),
                                Convert.ToDouble(tag.Attribute("lowLimit").Value),
                                Convert.ToDouble(tag.Attribute("highLimit").Value),
                                (Unit)Enum.Parse(typeof(Unit), tag.Attribute("unit").Value)
                            ));
                        break;
                    default:
                        break;
                }
            }

            return tagList;
        }
    }
}