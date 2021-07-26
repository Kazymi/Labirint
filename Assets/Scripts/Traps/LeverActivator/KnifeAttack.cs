using DG.Tweening;

public class KnifeAttack : LeaverActivator
{
    public override void Activate()
    {
        transform.DOMoveY(0.7f, 0.3f);
    }

    public override void Deactivate()
    {
        transform.DOMoveY(0f, 0.3f);
    }
}
