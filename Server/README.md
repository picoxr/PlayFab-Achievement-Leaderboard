## Server setup

This part describes Cloud Scripts used on PlayFab console and Rules configured to trgger Achievements, as well as Leaderboard setup.

The Cloud script is based on PlayFab's default Revision, adding 3 functions to excute:
* checkHighScoreForAchievement(): Grant user high score achievement, associated with PlayStream Rule checkHighScoreForAchievement. The threshhold is defined in Rule as follow.
![checkHighScoreForAchievementRule](https://user-images.githubusercontent.com/46362299/134811383-a17352b1-1553-437a-926a-8c678674dfd3.png)
* checkLevelCompleteForAchievement(): Grant level complete achievement, associated with PlayStream Rule checkLevelCompleteForAchievement. The threshhold is defined in Rule as follow.
![checkLevelCompleteForAchievementRule](https://user-images.githubusercontent.com/46362299/135022522-1fbd647f-21ca-40d1-acb7-9b87a00bf718.png)
* initializePlayer(): Initialize statistic attribute HighScore and achievement initial status, associated with Rule HighScoreLeaderboard.
![InitializePlayerDataRule](https://user-images.githubusercontent.com/46362299/134811393-2d3f08bb-3f8d-42a7-b9df-d2a4d209e6e6.png)

Leaderboard Setup as follow:
![HighScoreLeaderboard](https://user-images.githubusercontent.com/46362299/134811389-7f6ea29f-d532-454f-ac94-ae75b0c15f07.png)
