public class BellPuzzlePiece : SoundInteractable
{
    protected override void OnShotBehaviour()
    {
        base.OnShotBehaviour();
        GameManager.Instance.RegisterBellHit(this);
    }
}