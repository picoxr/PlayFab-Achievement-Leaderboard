## Server setup

This part describes Cloud Scripts used on PlayFab console and Rules configured to trgger Achievements, as well as Leaderboard setup.

The Cloud script is based on PlayFab's default Revision, adding 3 functions to excute:
* checkHighScoreForAchievement(): Grant user high score achievement, associate with PlayStream Rule checkHighScoreForAchievement. The threshhold is defined in Rule as follow.

* checkLevelCompleteForAchievement(): Grant level complete achievement. Currently it's executed on client's request.
* initializePlayer(): Initialize statistic attribute HighScore and achievement initial status, associated with Rule HighScoreLeaderboard.

Leaderboard Setup as follow:
