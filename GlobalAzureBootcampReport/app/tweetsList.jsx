import React, {Component, PropTypes} from 'react';
import {Panel} from 'react-bootstrap';

import ApiService from './apiService.js';
import BootcampManager from './bootcampManager.js';

class TweetsList extends Component {
  constructor(props) {
    super(props);

    this.state = {
      tweets: []
    };
    this.onUpdateTimeline = this.onUpdateTimeline.bind(this);
  }

  componentDidMount() {
    ApiService.get('api/data/latestTweets')
              .then(data => this.setState({tweets: data}))
    BootcampManager.addTimelineChangeListener(this.onUpdateTimeline);
  }

  onUpdateTimeline(newTweets) {
    this.setState({tweets: [...newTweets, ...this.state.tweets]});
  }

  render() {
		return (
			<Panel className="userStats" header='Users Statistcis' bsStyle='info'>
				<ul>
					{this.state.tweets.map(tweet =>
            <div key={tweet.id} className="tweetContainer">
              <div className="tweetHeader">
                <a href={`https://www.twitter.com/${tweet.createdBy.screenName}`} target="_blank">@{tweet.createdBy.name}</a>
              </div>
              <div className="tweetBody">{tweet.text}</div>
              <div className="tweetFooter">
                <a href={`https://www.twitter.com/${tweet.createdBy.screenName}/status/${tweet.id}`} target="_blank">View</a>
                <span>{tweet.createdAt}</span>
              </div>
              <hr/>
            </div>
          )}
				</ul>
			</Panel>
		);
  }
}

export default TweetsList;
