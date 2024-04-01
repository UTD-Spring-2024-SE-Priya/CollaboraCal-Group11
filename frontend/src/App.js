import React from 'react';
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import LoginSignUp from './Components/LoginSignUp/LoginSignUp';
import Main from './Components/Main/Main';
import ForgotPassword from './Components/ForgotPassword/ForgotPassword';

function App() {
  return (
    <Router>
      <div>
        <Routes>
          <Route path="/" element={<LoginSignUp />} />
          <Route path="/home" element={<Main />} />
          <Route path="/reset-password" element={<ForgotPassword />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
