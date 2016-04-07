import React from 'react';
import {Grid, Row, Col} from 'react-bootstrap';

import './style.css';

import NavigationBar from './navigationBar.jsx';
import UsersStatsList from './usersStatsList.jsx';

const App = () => (
<div>
  <NavigationBar />
  <Grid fluid={false} >
    <Row>
      <Col xs={12} md={3}>
        <UsersStatsList />
      </Col>
      <Col xs={12} md={6}>
        tweets list
      </Col>
      <Col xs={12} md={3}>
        security
      </Col>
    </Row>
  </Grid>
</div>
);

export default App;
