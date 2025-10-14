/// <summary>
/// Interface for objects that can respond to player interactions.
/// Implement this on any object that should have an interactive behavior (e.g., doors, switches, items).
/// </summary>
public interface IInteractable
{
    public string InteractionPrompt { get; set; }

    /// <summary>
    /// Called when the player interacts with this object.
    /// Implement the desired interaction logic inside this method.
    /// </summary>
    public void Interact();
}
