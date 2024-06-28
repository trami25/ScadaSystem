using DriverApi;
using RTDriver;
using ScadaCore.Tags.Model;
using SimulationDriver;
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
        private string _configPath = @"../../scadaConfig.xml";

        public ScadaConfiguration(string configPath, MainSimulationDriver simulationDriver, RTDriver.RTDriver rtuDriver)
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

        public void AddTag(Tag tag)
        {
            XElement tagElement = new XElement(tag is DigitalInputTag ? "di" :
                                               tag is DigitalOutputTag ? "do" :
                                               tag is AnalogInputTag ? "ai" :
                                               tag is AnalogOutputTag ? "ao" : "unknown");

            tagElement.Add(new XAttribute("id", tag.Id));
            tagElement.Add(new XAttribute("description", tag.Description));
            tagElement.Add(new XAttribute("ioAddress", tag.IOAddress));

            if (tag is DigitalInputTag diTag)
            {
                tagElement.Add(new XAttribute("scanTime", diTag.ScanTime));
                tagElement.Add(new XAttribute("isScanOn", diTag.IsScanOn));
                tagElement.Add(new XAttribute("driver", diTag.Driver is MainSimulationDriver ? "simulation" : "rtu"));
                tagElement.Add(new XText(diTag.Value.ToString()));
            }
            else if (tag is DigitalOutputTag doTag)
            {
                tagElement.Add(new XAttribute("initialValue", doTag.InitialValue));
                tagElement.Add(new XText(doTag.Value.ToString()));
            }
            else if (tag is AnalogInputTag aiTag)
            {
                tagElement.Add(new XAttribute("scanTime", aiTag.ScanTime));
                tagElement.Add(new XAttribute("isScanOn", aiTag.IsScanOn));
                tagElement.Add(new XAttribute("lowLimit", aiTag.LowLimit));
                tagElement.Add(new XAttribute("highLimit", aiTag.HighLimit));
                tagElement.Add(new XAttribute("unit", aiTag.Unit));
                tagElement.Add(new XAttribute("driver", aiTag.Driver is MainSimulationDriver ? "simulation" : "rtu"));
                tagElement.Add(new XText(aiTag.Value.ToString()));

                foreach (var alarm in aiTag.Alarms)
                {
                    XElement alarmElement = new XElement("alarm");
                    alarmElement.Add(new XAttribute("id", alarm.TagName));
                    alarmElement.Add(new XAttribute("priority", alarm.Priority));
                    alarmElement.Add(new XAttribute("threshold", alarm.Threshold));
                    alarmElement.Add(new XAttribute("type", alarm.Type));
                    tagElement.Add(alarmElement);
                }
            }
            else if (tag is AnalogOutputTag aoTag)
            {
                tagElement.Add(new XAttribute("initialValue", aoTag.InitialValue));
                tagElement.Add(new XAttribute("lowLimit", aoTag.LowLimit));
                tagElement.Add(new XAttribute("highLimit", aoTag.HighLimit));
                tagElement.Add(new XAttribute("unit", aoTag.Unit));
                tagElement.Add(new XText(aoTag.Value.ToString()));
            }

            _xml.Element("tags").Add(tagElement);
            Save();
        }

        public void AddAlarmToTag(string tagId, Alarm alarm)
        {
            var tagElement = _xml.Element("tags").Elements().FirstOrDefault(e => e.Attribute("id").Value == tagId);

            if (tagElement != null)
            {
                XElement alarmElement = new XElement("alarm");
                alarmElement.Add(new XAttribute("id", alarm.TagName));
                alarmElement.Add(new XAttribute("priority", alarm.Priority));
                alarmElement.Add(new XAttribute("threshold", alarm.Threshold));
                alarmElement.Add(new XAttribute("type", alarm.Type));
                tagElement.Add(alarmElement);
                Save();
            }
        }

        public void UpdateTag(Tag tag)
        {
            var tagElement = _xml.Element("tags").Elements().FirstOrDefault(e => e.Attribute("id").Value == tag.Id);

            if (tagElement != null)
            {
                tagElement.SetAttributeValue("description", tag.Description);
                tagElement.SetAttributeValue("ioAddress", tag.IOAddress);

                if (tag is DigitalInputTag diTag)
                {
                    tagElement.SetAttributeValue("scanTime", diTag.ScanTime);
                    tagElement.SetAttributeValue("isScanOn", diTag.IsScanOn);
                    tagElement.SetAttributeValue("driver", diTag.Driver is MainSimulationDriver ? "simulation" : "rtu");
                    tagElement.SetValue(diTag.Value.ToString());
                }
                else if (tag is DigitalOutputTag doTag)
                {
                    tagElement.SetAttributeValue("initialValue", doTag.InitialValue);
                    tagElement.SetValue(doTag.Value.ToString());
                }
                else if (tag is AnalogInputTag aiTag)
                {
                    tagElement.SetAttributeValue("scanTime", aiTag.ScanTime);
                    tagElement.SetAttributeValue("isScanOn", aiTag.IsScanOn);
                    tagElement.SetAttributeValue("lowLimit", aiTag.LowLimit);
                    tagElement.SetAttributeValue("highLimit", aiTag.HighLimit);
                    tagElement.SetAttributeValue("unit", aiTag.Unit);
                    tagElement.SetAttributeValue("driver", aiTag.Driver is MainSimulationDriver ? "simulation" : "rtu");
                    tagElement.SetValue(aiTag.Value.ToString());

                    var existingAlarms = tagElement.Descendants("alarm").ToList();
                    foreach (var alarmElement in existingAlarms)
                    {
                        alarmElement.Remove();
                    }

                    foreach (var alarm in aiTag.Alarms)
                    {
                        XElement alarmElement = new XElement("alarm");
                        alarmElement.Add(new XAttribute("id", alarm.TagName));
                        alarmElement.Add(new XAttribute("priority", alarm.Priority));
                        alarmElement.Add(new XAttribute("threshold", alarm.Threshold));
                        alarmElement.Add(new XAttribute("type", alarm.Type));
                        tagElement.Add(alarmElement);
                    }
                }
                else if (tag is AnalogOutputTag aoTag)
                {
                    tagElement.SetAttributeValue("initialValue", aoTag.InitialValue);
                    tagElement.SetAttributeValue("lowLimit", aoTag.LowLimit);
                    tagElement.SetAttributeValue("highLimit", aoTag.HighLimit);
                    tagElement.SetAttributeValue("unit", aoTag.Unit);
                    tagElement.SetValue(aoTag.Value.ToString());
                }

                Save();
            }
        }

        public void DeleteTag(string tagId)
        {
            var tagElement = _xml.Element("tags").Elements().FirstOrDefault(e => e.Attribute("id").Value == tagId);

            if (tagElement != null)
            {
                tagElement.Remove();
                Save();
            }
        }

        public void DeleteAlarmFromTag(string tagId, string alarmId)
        {
            var tagElement = _xml.Element("tags").Elements().FirstOrDefault(e => e.Attribute("id").Value == tagId);

            if (tagElement != null)
            {
                var alarmElement = tagElement.Descendants("alarm").FirstOrDefault(a => a.Attribute("id").Value == alarmId);
                if (alarmElement != null)
                {
                    alarmElement.Remove();
                    Save();
                }
            }
        }

        private void Save()
        {
            _xml.Save(_configPath);
        }
    }
}
