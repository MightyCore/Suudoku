using System.Linq;
using System.Collections.Generic;

namespace SuudokuAnalysisTry.Calc
{
    class Calc
    {
        public bool Exe()
        {
            if (Map.Reset) return false;
            while (true)
            {
                // 行、列、エリアの残1セルに番号記入
                if (Map.Targets.Any(x =>
                {
                    var wCells = Map.GetZeroCellsWithoutNum(x.Num);
                    return wCells.Count(y => y.SetNumWhenTheLastOne(x.Num, wCells)) > 0;
                    // 記入セルがあれば再開
                })) continue;
                break;
            }

            // 値を仮で記入
            var wRemainCells = Map.Targets.SelectMany(x => Map.GetZeroCellsWithoutNum(x.Num)).Distinct().OrderBy(x => x.RemainNum().Count()).ToList();
            if (wRemainCells.Count > 0)
            {
                var wCell = wRemainCells.First();
                var wCopyCells = Map.Cells.DeepClone() as List<Map.Cell>;
                foreach (var wNum in wCell.RemainNum())
                {
                    wCell.SetNum(wNum);
                    if (Exe()) return true;
                    Map.Reset = false;
                    Map.Cells = wCopyCells;
                    wCell = Map.Cells.Find(x => x.Row == wCell.Row && x.Col == wCell.Col && x.Area == wCell.Area);
                }
            }

            if (!Map.Reset && Map.Targets.Count > 0) Exe();

            return !Map.Reset;
        }
    }
}
