let usersStatsCallback;
let timeLineCallback;

export const updateUsersStats = (newStats) => {
  if (usersStatsCallback) {
    usersStatsCallback(newStats);
  }
};

export const updateTimeline = (newTweets) => {
  if (timeLineCallback) {
    timeLineCallback(newTweets);
  }
};

export default {
  addTimelineChangeListener(callback) {
    timeLineCallback = callback;
  },

  addUsersStatsChangeListener(callback) {
    usersStatsCallback = callback;
  },
}
