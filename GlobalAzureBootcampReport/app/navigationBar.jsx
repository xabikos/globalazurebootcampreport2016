import React from 'react';
import {Navbar, Nav, NavItem} from 'react-bootstrap';

import logo from './logo.png';

const NavigationBar = () => (
  <Navbar fixedTop={true} >
    <Navbar.Header>
      <Navbar.Brand>
        <a href="/"><img src={logo}/></a>
      </Navbar.Brand>
      <Navbar.Toggle />
    </Navbar.Header>
    <Nav pullRight>
      <NavItem href='http://huizingen-be.azurebootcamp.net/' target='_blank'>Azure Bootcamp</NavItem>
      <NavItem href='https://github.com/xabikos/globalazurebootcampreport2016' target='_blank'>Github</NavItem>
    </Nav>
  </Navbar>
);

export default NavigationBar;
