using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlRegisterIndexing
{
    public struct ControlRegister
    {
        private int registerData;

        /// <summary>
        /// Enables direct access to the registerData field.
        /// </summary>
        public int RegisterData
        {
            get
            {
                return registerData;
            }
            set
            {
                registerData = value;
            }
        }

        public int this[int index]
        {
            get
            {
                bool isSet = (registerData & (1 << index)) != 0;
                return isSet ? 1 : 0;
            }
            set
            {
                if (value != 0 && value != 1)
                {
                    throw new ArgumentException("Argument must be 1 or 0");
                }
                if (value == 1)
                    registerData |= (1 << index);
                else
                    registerData &= ~(1 << index);
            }
        }
    }
}
