import React, {Component} from 'react';
import {Grid, Row, Col} from 'react-bootstrap';

import './style.css';

import NavigationBar from './navigationBar.jsx';
import UsersStatsList from './usersStatsList.jsx';
import TweetsList from './tweetsList.jsx';
import UserTweetsList from './userTweetsList.jsx';

class App extends Component {
  constructor(props) {
    super(props);

    this.state = {
      selectedUserId: '',
    };
    this.handleViewUserTweets = this.handleViewUserTweets.bind(this);
  }

  handleViewUserTweets(userId) {
    this.setState({selectedUserId: userId});
  }

  render() {
    return (
      <div>
        <NavigationBar />
          <Grid fluid={false} >
            <Row>
              <Col xs={12} md={3}>
                <UsersStatsList onViewUsersTweets={this.handleViewUserTweets}/>
              </Col>
              <Col xs={12} md={6}>
                <TweetsList />
              </Col>
              <Col xs={12} md={3}>
                {<UserTweetsList userId={this.state.selectedUserId} />}
              </Col>
            </Row>
          </Grid>
      </div>
    );
  }
}

export default App;
