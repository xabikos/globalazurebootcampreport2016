import React, {Component, PropTypes} from 'react';
import {Panel} from 'react-bootstrap';

import ApiService from './apiService.js';

class UsersStatsList extends Component {
  constructor(props) {
    super(props);

    this.state = {
      stats: []
    };
  }

  componentDidMount() {
    ApiService.get('api/statistics')
              .then(data => this.setState({stats: data}))
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
					<div className="userCounter">{userStat.tweetsNumber} tweets</div>
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

UsersStatsList.propTypes = {

};

export default UsersStatsList;
