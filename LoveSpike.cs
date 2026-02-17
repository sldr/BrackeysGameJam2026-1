using Godot;
using System;

public partial class LoveSpike : TileMapLayer
{

    private int _Count = 1;
    private bool _NeedsApplyCount = true;

    [Export(PropertyHint.Range, "1,16")]
    public int Count
    {
        get => _Count;
        set
        {
            if (_Count != value) {
                _Count = value;
                // IMPORTANT: guard against scene not being ready yet
                if (!IsInsideTree()) {
                    this._NeedsApplyCount = true;
                    return;
                }
                ApplyCount();
            }
        }
    }

    private void ApplyCount()
    {
        Vector2I startPos = new Vector2I(0, 0);

        // Get the tile data from (0,0)
        int sourceId = GetCellSourceId(startPos);
        if (sourceId == -1)
            return; // No tile at 0,0

        Vector2I atlasCoords = GetCellAtlasCoords(startPos);
        int alternative = GetCellAlternativeTile(startPos);

        // Duplicate horizontally
        for (int x = 1; x < Count; x++) {
            Vector2I pos = new Vector2I(x, 0);
            SetCell(pos, sourceId, atlasCoords, alternative);
        }
    }

    public override void _Ready()
    {
        base._Ready();
        if (_NeedsApplyCount) {
            ApplyCount();
        }

    }
}
