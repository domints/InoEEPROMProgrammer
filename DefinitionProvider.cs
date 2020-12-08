using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace InoEEPROMProgrammer
{
    public class DefinitionProvider
    {
        private readonly Dictionary<string, EEPROMDefinition> _definitions;
        public DefinitionProvider()
        {
            _definitions = JsonConvert
                .DeserializeObject<Dictionary<string, EEPROMDefinition>>(
                    File.ReadAllText("eepromDefs.json")
                );
        }

        public EEPROMDefinition Get(string name)
        {
            if(_definitions.ContainsKey(name.ToLowerInvariant()))
                return _definitions[name.ToLowerInvariant()];

            return null;
        }
    }
}