using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;
//
// [Puzzle(Year = 2021, Day = 4, Part = 1)]
// public class Day04A : IPuzzle
// {
//     private List<int> _drawnNumbers = new();
//     private List<BingoCard> _bingoCards = new();    
//
//     public async Task ParseInput(TextReader reader)
//     {
//         _drawnNumbers = (await reader.ReadLineAsync())!.Split(',').Select(int.Parse).ToList();
//         await reader.ReadLineAsync();
//         
//         while (reader.Peek() > 0)
//         {
//             _bingoCards.Add(await BingoCard.Load(reader));
//             if (reader.Peek() > 0)
//             {
//                 await reader.ReadLineAsync();
//             } 
//         }
//     }
//
//     public Task<Output> Run()
//     {
//         foreach (var number in _drawnNumbers)
//         {
//             foreach (var card in _bingoCards)
//             {
//                 int? score = card.MarkOff(number);
//                 if (score.HasValue)
//                 {
//                     return Task.FromResult<Output>(score);
//                 }
//             }
//         }
//
//         throw new Exception();
//     }
//     
//     public class BingoCard
//     {
//         private int?[][] numbers;
//     
//         public BingoCard(int?[][] numbers)
//         {
//             this.numbers = numbers;
//         }
//     
//         public static async Task<BingoCard> Load(TextReader reader)
//         {
//             var numbers = new int?[5][];
//             
//             for (int row = 0; row < 5; row++)
//             {
//                 string line = await reader.ReadLineAsync();
//                 var parsed = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
//                 numbers[row] = parsed.Cast<int?>().ToArray();
//             }
//     
//             return new BingoCard(numbers);
//         }
//     
//         public int? MarkOff(int number)
//         {
//             var pos = Find(number);
//             if (!pos.HasValue) return null;
//     
//             bool won = Mark(pos.Value);
//             if (!won) return null;
//     
//             int remainingNumbers = numbers.Sum(col => col.Sum(x => x.GetValueOrDefault(0)));
//             return remainingNumbers * number;
//         }
//     
//         private (int row, int col)? Find(int number)
//         {
//             for (int row = 0; row < 5; row++)
//             {
//                 for (int col = 0; col < 5; col++)
//                 {
//                     if (numbers[row][col] == number)
//                     {
//                         numbers[row][col] = null;
//                         return (row, col);
//                     }
//                 }
//             }
//            
//             return default;
//         }
//     
//         private bool Mark((int row, int col) pos)
//         {
//             numbers[pos.row][pos.col] = null;
//             if (numbers[pos.row].All(nr => !nr.HasValue)) return true;
//             for (int i = 0; i < 5; i++)
//             {
//                 if (numbers[i][pos.col].HasValue) return false;
//             }
//             return true;
//         }
//     }
// }
