private bool _isEnable;

private void On()
{
    if (_isEnable)
        _effects.StartEnableAnimation();
}

private void Off()
{
    if (_isEnable == false)
        _pool.Free(this);
}