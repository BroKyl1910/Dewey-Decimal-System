using System.Collections.Generic;

namespace DeweyDecimalSystem.Models
{
    class CallNumberJSONModel
    {
        public Tree<CallNumber> CallNumberTree { get; set; }
        public List<CallNumber> Level1CallNumbers { get; set; }
        public List<CallNumber> Level2CallNumbers { get; set; }
        public List<CallNumber> Level3CallNumbers { get; set; }

        public CallNumberJSONModel()
        {
        }

        public CallNumberJSONModel(Tree<CallNumber> callNumberTree, List<CallNumber> level1CallNumbers, List<CallNumber> level2CallNumbers, List<CallNumber> level3CallNumbers)
        {
            CallNumberTree = callNumberTree;
            Level1CallNumbers = level1CallNumbers;
            Level2CallNumbers = level2CallNumbers;
            Level3CallNumbers = level3CallNumbers;
        }

    }
}
