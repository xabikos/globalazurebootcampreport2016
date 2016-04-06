import React, {Component, PropTypes} from 'react';
import {Panel} from 'react-bootstrap';


class UsersStatsList extends Component {
  constructor(props) {
    super(props);

    this.state = {
      stats: []
    };
  }


  render() {
    const stats = this.state.stats.map(userStat =>
			<li>
				<div className="userImage">
					<a href={userStat.ProfileUrl} target="_blank">
						<img src={userStat.ImageUrl} />
					</a>
				</div>
				<div className="userInfo">
					<div className="userName"><a href={userStat.ProfileUrl} target="_blank">@{userStat.Name}</a></div>
					<div className="userCounter">{userStat.TweetsNumber} tweets</div>
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
