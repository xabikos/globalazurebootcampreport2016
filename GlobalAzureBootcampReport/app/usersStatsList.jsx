import React, {Component, PropTypes} from 'react';
import {Panel, Button} from 'react-bootstrap';

import ApiService from './apiService.js';
import BootcampManager from './bootcampManager.js';

class UsersStatsList extends Component {
  constructor(props) {
    super(props);

    this.state = {
      stats: []
    };
    this.onUpdateUsersStats = this.onUpdateUsersStats.bind(this);
  }

  componentDidMount() {
    ApiService.get('api/data/usersStats')
              .then(data => this.setState({stats: data}))
    BootcampManager.addUsersStatsChangeListener(this.onUpdateUsersStats);
  }

  onUpdateUsersStats(newStats){
    this.setState({stats: newStats});
  }

  render() {
    const stats = this.state.stats.map(userStat =>
			<li key={userStat.userId}>
				<div className="userImage">
					<a href={userStat.profileUrl} target="_blank">
						<img src={userStat.imageUrl} />
					</a>
				</div>
				<div className="userInfo">
					<div className="userName"><a href={userStat.profileUrl} target="_blank">@{userStat.name}</a></div>
					<div className="userCounter">
            {userStat.tweetsNumber} tweets <Button onClick={() => this.props.onViewUsersTweets(userStat.userId)} bsStyle="link">View all</Button>
          </div>
				</div>
			</li>
		);

		return (
			<Panel className="userStats" header='Users Statistcis' bsStyle='info'>
				<ul>
					{stats}
				</ul>
			</Panel>
		);
  }
}

UsersStatsList.propTyeps = {
  onViewUsersTweets: PropTypes.func.isRequired,
};

export default UsersStatsList;
