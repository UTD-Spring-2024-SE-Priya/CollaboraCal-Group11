import React from 'react';
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import LoginSignUp from './Components/LoginSignUp/LoginSignUp';
import Main from './Components/Main/Main';

function App() {
  return (
    <Router>
      <div>
        <Routes>
          <Route path="/" element={<LoginSignUp />} />
          <Route path="/home" element={<Main />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
