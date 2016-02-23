using System;
using System.Collections.Generic;
using System.Linq;

namespace MWS.Helper
{
    public static class AssignmentBoardHelper
    {
        /// <summary>
        /// Add a Participant to the Assignment Board Hierarchy.
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="participantConfiguration"></param>
        /// <returns></returns>
        public static MovilizerParticipantConfiguration AddParticipantToHierarchy(List<Group> hierarchy, MovilizerParticipantConfiguration participantConfiguration)
        {
            participantConfiguration.attributeUpdate = new MovilizerAttributeEntry[] { GetParticipantAttributeUpdateFromGroupString(hierarchy) };
            foreach (Group group in hierarchy)
            {
                MovilizerParticipantGroup participantGroup = new MovilizerParticipantGroup();
                participantGroup.name = group.id;
                if (participantConfiguration.groupAdd == null)
                {
                    participantConfiguration.groupAdd = new MovilizerParticipantGroup[] { participantGroup };
                }
                else
                {
                    var temp = participantConfiguration.groupAdd.ToList();
                    temp.Add(participantGroup);
                    participantConfiguration.groupAdd = temp.ToArray();
                }
            }

            return participantConfiguration;
        }

        /// <summary>
        /// Generate MOVILIZER_ASSIGNMENT_BOARD_HIERARCHY from groups.
        /// </summary>
        /// <param name="groups">The groups which should be generated into a hierarchy.</param>
        /// <returns></returns>
        private static MovilizerAttributeEntry GetParticipantAttributeUpdateFromGroupString(List<Group> groups)
        {
            MovilizerAttributeEntry entry = new MovilizerAttributeEntry();
            entry.name = "MOVILIZER_ASSIGNMENT_BOARD_HIERARCHY";
            entry.useIndex = true;
            MovilizerGenericDataContainerEntry currentEntry = null;

            foreach (Group currentGroup in groups.OrderByDescending(group => group.level))
            {
                if (currentEntry == null)
                {
                    currentEntry = new MovilizerGenericDataContainerEntry();
                    currentEntry.name = ("level-" + currentGroup.level);
                    currentEntry.valstr = currentGroup.name;

                    var groupEntry = new MovilizerGenericDataContainerEntry();
                    groupEntry.name = "GROUP";
                    groupEntry.valstr = currentGroup.id;

                    var templist = new List<MovilizerGenericDataContainerEntry>();
                    templist.Add(groupEntry);
                    currentEntry.entry = templist.ToArray();
                }
                else {
                    MovilizerGenericDataContainerEntry tempEntry = new MovilizerGenericDataContainerEntry();

                    tempEntry.name = ("level-" + currentGroup.level);
                    tempEntry.valstr = currentGroup.name;

                    var groupEntry = new MovilizerGenericDataContainerEntry();
                    groupEntry.name = "GROUP";
                    groupEntry.valstr = currentGroup.id;

                    var templist = new List<MovilizerGenericDataContainerEntry>();
                    templist.Add(groupEntry);
                    templist.Add(currentEntry);
                    tempEntry.entry = templist.ToArray();

                    currentEntry = tempEntry;
                }
            }

            MovilizerGenericDataContainer container = new MovilizerGenericDataContainer();
            container.entry = new MovilizerGenericDataContainerEntry[] { currentEntry };

            entry.Item = container;
            return entry;
        }
    }

    public class Group
    {
        public String name { get; set; }
        public String id { get; set; }
        public Int32 level { get; set; }
    }
}
