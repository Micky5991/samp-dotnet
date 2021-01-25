using System;
using Dawn;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Elements.Entities
{
    /// <inheritdoc cref="IPlayer"/>
    public partial class Player
    {
        /// <inheritdoc />
        public void SetPVar(string varname, string value)
        {
            Guard.Argument(varname, nameof(varname)).NotNull().NotEmpty().MaxLength(40);
            Guard.Argument(value, nameof(value)).NotNull();
            Guard.Disposal(this.Disposed);

            this.playersNatives.SetPVarString(this.Id, varname, value);
        }

        /// <inheritdoc />
        public void SetPVar(string varname, int value)
        {
            Guard.Argument(varname, nameof(varname)).NotNull().NotEmpty().MaxLength(40);
            Guard.Disposal(this.Disposed);

            this.playersNatives.SetPVarInt(this.Id, varname, value);
        }

        /// <inheritdoc />
        public void SetPVar(string varname, float value)
        {
            Guard.Argument(varname, nameof(varname)).NotNull().NotEmpty().MaxLength(40);
            Guard.Disposal(this.Disposed);

            this.playersNatives.SetPVarFloat(this.Id, varname, value);
        }

        /// <inheritdoc />
        public void SetPVar(string varname, object value)
        {
            Guard.Argument(varname, nameof(varname)).NotNull().NotEmpty().MaxLength(40);
            Guard.Disposal(this.Disposed);

            switch (value)
            {
                case int i:
                    this.SetPVar(varname, i);

                    break;

                case string s:
                    this.SetPVar(varname, s);

                    break;

                case float f:
                    this.SetPVar(varname, f);

                    break;

                default:
                    throw new ArgumentException($"The type {value.GetType()} is not accepted.", nameof(value));
            }
        }

        /// <inheritdoc />
        public T? GetPVar<T>(string varname)
        {
            Guard.Argument(varname, nameof(varname)).NotNull().NotEmpty().MaxLength(40);
            Guard.Disposal(this.Disposed);

            var type = this.GetPVarType(varname);

            switch (type)
            {
                case PlayerVartype.Int when typeof(T) == typeof(int):
                    break;

                case PlayerVartype.String when typeof(T) == typeof(string):
                    break;

                case PlayerVartype.Float when typeof(T) == typeof(float):
                    break;
            }

            return default;
        }

        /// <inheritdoc />
        public bool DeletePVar(string varname)
        {
            Guard.Argument(varname, nameof(varname)).NotNull().NotEmpty().MaxLength(40);
            Guard.Disposal(this.Disposed);

            return this.playersNatives.DeletePVar(this.Id, varname);
        }

        /// <inheritdoc />
        public int GetPVarsUpperIndex()
        {
            Guard.Disposal(this.Disposed);

            return this.playersNatives.GetPVarsUpperIndex(this.Id);
        }

        /// <inheritdoc />
        public void GetPVarNameAtIndex(int index, out string varname)
        {
            Guard.Argument(index, nameof(index)).NotNegative();
            Guard.Disposal(this.Disposed);

            this.playersNatives.GetPVarNameAtIndex(this.Id, index, out varname, 40);
        }

        /// <inheritdoc />
        public PlayerVartype GetPVarType(string varname)
        {
            Guard.Argument(varname, nameof(varname)).NotNull().NotEmpty().MaxLength(40);
            Guard.Disposal(this.Disposed);

            return (PlayerVartype)this.playersNatives.GetPVarType(this.Id, varname);
        }
    }
}
