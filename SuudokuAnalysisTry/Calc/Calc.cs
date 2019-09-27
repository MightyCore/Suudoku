using System.Linq;
using System.Collections.Generic;

namespace SuudokuAnalysisTry.Calc
{
    class Calc
    {
        /// <summary>
        /// メイン処理
        /// </summary>
        public void Exe(long vAnsLimit)
        {
            Map.Ansers.Clear();

            while (Map.Ansers.Count < vAnsLimit)
            {
                #region FirstBlock
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
                #endregion

                #region SecondBlock
                // 0セルを、候補数値の少ない順に取得
                var wRemainCells = Map.Targets.SelectMany(x => Map.GetZeroCellsWithoutNum(x)).Distinct().OrderBy(x => x.RemainNum().Count()).ToList();
                if (wRemainCells.Count > 0)
                {
                    var wCell = wRemainCells.First();
                    if(wCell.TempCnt < wCell.RemainNum().Count)
                    {
                        // 仮で番号を記入
                        var wNum = wCell.RemainNum()[wCell.TempCnt];
                        wCell.SetNumTemp(wNum);
                    }
                    else
                    {
                        // 仮番号の初期化
                        wCell.TempCnt = 0;
                        Map.ClearNumTemp();
                    }

                    // 回答が出きったら終了
                    if (Map.CellsIndexNumbered.Count == 0) break;

                    // FirstBlockの処理を再開
                    continue;
                }
                #endregion

                // 回答が出きったら終了
                if (Map.CellsIndexNumbered.Count == 0) break;

                // 全ての値の記入が完了したら回答群に追加
                if (Map.Targets.Count == 0) Map.Ansers.Add(Map.Cells.DeepClone() as List<Map.Cell>);

                // 仮番号を1階層初期化
                Map.ClearNumTemp();
            }

            // 後処理
            Map.CellsIndexNumbered.Clear();
            Map.CellsIndexNumbered.Add(new List<int>());
        }
    }
}
