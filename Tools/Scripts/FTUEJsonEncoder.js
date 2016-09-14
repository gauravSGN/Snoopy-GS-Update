/*
 * Exports JSON representation of FTUE config spreadsheet.
 *
 * Example: =ftueJsonEncoder(Sheet2!A2:G2,Sheet1!A2:G10)
 */
function ftueJsonEncoder(rewardCells, tutorialCells) {
  var triggerMap = {
    "Level_start": 0,
    "Complete_level": 1,
    "Pre_level_popup": 2,
    "Post_level_popup": 3,
    "Saga_map": 4,
    "Out_of_moves": 5,
    "Lantern_fill": 6,
    "Tutorial_complete": 7
  };

  return JSON.stringify({
    rewardLists: rewardCells.map(function (row) {
      var length = row.length;
      var result = [];

      for (var index = 1; index < length; index += 2)
      {
        if (row[index]) {
          result.push({item: row[index], count: row[index + 1]});
        }
      }

      return { id: row[0], rewards: result };
    }),
    tutorials: tutorialCells.map(function (row) {
      return {
        id: row[0],
        trigger: triggerMap[row[1]],
        level: row[2],
        skippable: row[3] == "Yes",
        config: row[4],
        reward: row[5],
        text: row[6]
      }
    })
  });
}
