using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateGraph
{
    /// <summary>
    /// Class that holds a list of stress analysis results for a given temperature
    /// </summary>
    public class StressData
    {
        /// <summary>
        /// The temperature at which the stress test results were generated. 
        /// <para>
        /// This value is specified in Kelvin (K).
        /// </para>
        /// </summary>
        public short Temperature { get; set; }

        /// <summary>
        /// A list of applied stress and deflection pairs recorded by the test.
        /// <para>
        /// The applied stress values are used as the dictionary keys, specified in kilo-Newtons (kN).
        /// The deflection data are used as the dictionary values, specified in millimeters (mm).
        /// Given an applied stress value, the deflection for that stress can be retrieved.
        /// </para>
        /// </summary>
        public Dictionary<short, short?> Data { get; set; }

        /// <summary>
        /// Public method that renders the data in a StressData object as a string.
        /// </summary>
        /// <returns>
        /// A string representation of the data in the StressData object
        /// </returns>
        public override string ToString()
        {
            StringBuilder stringData = new StringBuilder();
            stringData.Append(String.Format("Temperature: {0}K\n", this.Temperature));
            foreach (var item in this.Data)
            {
                stringData.Append(String.Format("Stress: {0}kN\t\tDeflection: {1}mm\n", item.Key, item.Value));
            }

            return stringData.ToString();
        }
    }
}
