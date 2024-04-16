import React from 'react';
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import LoginSignUp from './Components/LoginSignUp/LoginSignUp';
import Main from './Components/Main/Main';
import ForgotPassword from './Components/ForgotPassword/ForgotPassword';
import CreateEvent from './Components/CreateEvent/CreateEvent';
import CreateCalendar from './Components/CreateCalendar/CreateCalendar';

function App() {
  return (
    <Router>
      <div>
        <Routes>
          <Route path="/" element={<LoginSignUp />} />
          <Route path="/home" element={<Main />} />
          <Route path="/reset-password" element={<ForgotPassword />} />
          <Route path="/create-event" element={<CreateEvent />} />
          <Route path="/create-calendar" element={<CreateCalendar />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
