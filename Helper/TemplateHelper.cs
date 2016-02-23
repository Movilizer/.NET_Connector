using System;

namespace MWS.Helper
{
    public static class TemplateHelper
    {
        private static readonly Type MoveletType = typeof (MovilizerMovelet);
        private static readonly Type MasterdataUpdateType = typeof(MovilizerMasterdataUpdate);
        private static readonly Type MasterdataDeleteType = typeof(MovilizerMasterdataDelete);


        /**
         * MOVELET
         */
        public static MovilizerMovelet DeserializeMoveletFromFile(string filename, string defaultNameSpace) => 
            XmlHelper.DeserializeFromFile(filename, MoveletType, defaultNameSpace) as MovilizerMovelet;

        public static MovilizerMovelet DeserializeMoveletFromResourceCode(string resourcePrefix, string resourceCode, string defaultNameSpace) => 
            XmlHelper.DeserializeFromResourceCode(resourcePrefix + resourceCode, MoveletType, defaultNameSpace) as MovilizerMovelet;

        public static MovilizerMovelet DeserializeMoveletFromResourceCode(string resourceCode, string defaultNameSpace) =>
            XmlHelper.DeserializeFromResourceCode(resourceCode, MoveletType, defaultNameSpace) as MovilizerMovelet;

        /**
         * MASTER DATA UPDATE
         */
        public static MovilizerMasterdataUpdate DeserializeMasterdataUpdateFromFile(string filename, string defaultNameSpace) =>
            XmlHelper.DeserializeFromFile(filename, MasterdataUpdateType, defaultNameSpace) as MovilizerMasterdataUpdate;

        public static MovilizerMasterdataUpdate DeserializeMasterdataUpdateFromResourceCode(string resourcePrefix, string resourceCode, string defaultNameSpace) => 
            XmlHelper.DeserializeFromResourceCode(resourcePrefix + resourceCode, MasterdataUpdateType, defaultNameSpace) as MovilizerMasterdataUpdate;

        public static MovilizerMasterdataUpdate DeserializeMasterdataUpdateFromResourceCode(string resourceCode, string defaultNameSpace) =>
            XmlHelper.DeserializeFromResourceCode(resourceCode, MasterdataUpdateType, defaultNameSpace) as MovilizerMasterdataUpdate;

        /**
         * MASTER DATA DELETE
         */
        public static MovilizerMasterdataDelete DeserializeMasterdataDeleteFromFile(string filename, string defaultNameSpace) => 
            XmlHelper.DeserializeFromFile(filename, MasterdataDeleteType, defaultNameSpace) as MovilizerMasterdataDelete;

        public static MovilizerMasterdataDelete DeserializeMasterdataDeleteFromResourceCode(string resourcePrefix, string resourceCode, string defaultNameSpace) => 
            XmlHelper.DeserializeFromResourceCode(resourcePrefix + resourceCode, MasterdataDeleteType, defaultNameSpace) as MovilizerMasterdataDelete;

        public static MovilizerMasterdataDelete DeserializeMasterdataDeleteFromResourceCode(string resourceCode, string defaultNameSpace) => 
            XmlHelper.DeserializeFromResourceCode(resourceCode, MasterdataDeleteType, defaultNameSpace) as MovilizerMasterdataDelete;
    }
}
