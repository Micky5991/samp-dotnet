using Dawn;
using Micky5991.Samp.Net.Framework.Events;
using Micky5991.Samp.Net.Framework.Interfaces.TextDraws;

namespace Micky5991.Samp.Net.Framework.Elements.Entities
{
    /// <inheritdoc cref="Micky5991.Samp.Net.Framework.Interfaces.Entities.IPlayer"/>
    public partial class Player
    {
        /// <inheritdoc />
        public void ShowTextDraw(ITextDraw textDraw)
        {
            Guard.Argument(textDraw).NotNull();

            this.eventAggregator.Publish(
                                         new PlayerShowTextDrawEvent(
                                          this,
                                          textDraw));
        }

        /// <inheritdoc />
        public void HideTextDraw(ITextDraw textDraw)
        {
            Guard.Argument(textDraw).NotNull();

            this.eventAggregator.Publish(
                                         new PlayerHideTextDrawEvent(
                                          this,
                                          textDraw));
        }
    }
}
