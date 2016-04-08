import React from 'react';
import ReactDOM from 'react-dom';

import App from './app.jsx';
import 'expose?bootcampManager!./bootcampManager.js';

const reactAppNode = document.getElementById('react-app');
ReactDOM.render(<App />, reactAppNode);
