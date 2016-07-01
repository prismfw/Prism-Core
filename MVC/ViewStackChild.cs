using Prism.UI;

namespace Prism
{
    /// <summary>
    /// Defines an object, most commonly an <see cref="IView"/> instance, that will be pushed onto a <see cref="ViewStack"/>.
    /// </summary>
    public interface IViewStackChild
    {
        /// <summary>
        /// Gets or sets an identifier that determines the position in which this instance is placed in a <see cref="ViewStack"/>.
        /// Objects with the same identifier value will replace each other within the same <see cref="ViewStack"/>, and
        /// objects with different identifiers will be assigned different positions even if they are of the same type.
        /// </summary>
        string StackId { get; set; }
    }
}
