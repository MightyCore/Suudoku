using System.Linq;
using System.Collections.Generic;

namespace SuudokuAnalysisTry.Calc
{
    class Calc
    {
        /// <summary>
        /// メイン処理
        /// </summary>
        /// <returns>True:正常値、False：不整値入力あり</returns>
        public bool Exe()
        {
            // 誤った値が入力されていた場合、Falseを返す
            if (Map.Reset) return false;
            while (true)
            {
                // 行、列、エリアの残1セルに番号記入
                if (Map.Targets.Any(x =>
                {
                    var wCells = Map.GetZeroCellsWithoutNum(x);
                    return wCells.Count(y => y.SetNumWhenTheLastOne(x, wCells)) > 0;
                    // 記入セルがあれば再開
                })) continue;
                break;
            }

            // 0セルを、候補数値の少ない順に取得
            var wRemainCells = Map.Targets.SelectMany(x => Map.GetZeroCellsWithoutNum(x)).Distinct().OrderBy(x => x.RemainNum().Count()).ToList();
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

            // 誤った入力が無く、全セルが埋まっていない場合に、再起実行
            if (!Map.Reset && Map.Targets.Count > 0) Exe();

            // 誤った値が入力されていた場合、Falseを返す
            return !Map.Reset;
        }
    }
}
