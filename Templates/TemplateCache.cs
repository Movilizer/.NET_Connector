using System;
using System.Collections.Generic;

using MWS.Helper;
using MWS.WebService;

namespace MWS.Templates
{
    public class TemplateCache
    {
        private readonly Dictionary<string, object> _templates;

        // private constructor required for the singleton generic pattern
        private TemplateCache()
        {
            _templates = new Dictionary<string, object>();
        }

        private object GetTemplate(string key)
        {
            return _templates.ContainsKey(key) ? _templates[key] : null;
        }

        public MovilizerMovelet GetMovelet(string key)
        {
            return GetTemplate(key) as MovilizerMovelet;
        }

        public MoveletTemplate GetMoveletTemplate(string key)
        {
            return new MoveletTemplate(GetMovelet(key));
        }

        public MovilizerMasterdataUpdate GetMasterdataUpdate(string key)
        {
            return GetTemplate(key) as MovilizerMasterdataUpdate;
        }

        public MasterdataUpdateTemplate GetMasterdataUpdateTemplate(string key)
        {
            return new MasterdataUpdateTemplate(GetMasterdataUpdate(key));
        }

        public MovilizerMasterdataDelete GetMasterdataDelete(string key)
        {
            return GetTemplate(key) as MovilizerMasterdataDelete;
        }

        public MasterdataDeleteTemplate GetMasterdataDeleteTemplate(string key)
        {
            return new MasterdataDeleteTemplate(GetMasterdataDelete(key));
        }

        private void CacheTemplate(string key, object template)
        {
            if (template == null)
            {
                Console.WriteLine("Unable to deserialize XML file: " + key);
                return;
            }

            _templates.Add(key, template);
        }

        public void LoadMoveletTemplate(string prefix, string key)
        {
            // read the movelet from resource
            MovilizerMovelet movelet = TemplateHelper.DeserializeMoveletFromResourceCode(
                prefix, key, MovilizerWebServiceConstants.WS_NAMESPACE);

            CacheTemplate(key, movelet);
        }

        public void LoadMasterdataUpdateTemplate(string prefix, string key)
        {
            // read the masterdata update from resource
            MovilizerMasterdataUpdate masterdataUpdate = TemplateHelper.DeserializeMasterdataUpdateFromResourceCode(
                prefix, key, MovilizerWebServiceConstants.WS_NAMESPACE);

            CacheTemplate(key, masterdataUpdate);
        }

        public void LoadMasterdataDeleteTemplate(string prefix, string key)
        {
            // read the masterdata delete from resource
            MovilizerMasterdataDelete masterdataDelete = TemplateHelper.DeserializeMasterdataDeleteFromResourceCode(
                prefix, key, MovilizerWebServiceConstants.WS_NAMESPACE);

            CacheTemplate(key, masterdataDelete);
        }
    }
}
