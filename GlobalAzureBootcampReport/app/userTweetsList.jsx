import React, {Component, PropTypes} from 'react';
import {Panel} from 'react-bootstrap';
import Loader from 'react-loader';

import ApiService from './apiService.js';

class UserTweetsList extends Component {
  constructor(props) {
    super(props);

    this.state = {
      tweets: [],
      fetchingData: false,
    };
  }

  componentWillReceiveProps(nextProps) {
    this.setState({fetchingData: true});
    if (nextProps.userId !== this.props.userId) {
      ApiService.get(`api/data/getUserTweets?userId=${nextProps.userId}`)
        .then(data => this.setState({tweets: data, fetchingData: false}))
    }
  }

  render() {
    const header = (
      <span><strong>{this.state.tweets[0] && this.state.tweets[0].createdBy.name}</strong> User Tweets</span>
    );
    var loaderOptions = {
      top: '21%',
      left: '87%',
      scale: 0.60
    };
    return (
      <Panel className="userTweets" header={header} bsStyle='info'>
        {this.state.tweets.length === 0 && 'Please select View all from the User Statistics list to view user\'s tweets' }
        <Loader loaded={!this.state.fetchingData} options={loaderOptions}>
          <ul>
            {this.state.tweets.map(tweet =>
              <li key={tweet.id}>
                <div className="tweetContainer">
                  {tweet.text}
                  <div className="tweetFooter">
                    <p>{tweet.createdAt}</p>
                  </div>
                  <div>
                    <span>Favorites: {tweet.favoriteCount} </span>
                    <span>Retweets: {tweet.retweetCount}</span>
                  </div>
                  <hr />
                </div>
              </li>
            )}
          </ul>
        </Loader>
			</Panel>
    );
  }
}

UserTweetsList.propTypes = {
  userId: PropTypes.string.isRequired,
};

export default UserTweetsList;
