import React from 'react';
import { Button, Container, Row, Col, Card } from 'react-bootstrap';
import './Home.css'; // For custom styling

const Home = () => {
  return (
    <div className="home-page">
      {/* Hero Section */}
      <div className="hero-section d-flex align-items-center justify-content-center text-center">
        <Container>
          <h1 className="display-4 text-white">Transform Your Body, Transform Your Life</h1>
          <p className="lead text-white">Join our community and start your fitness journey today!</p>
          
          {/* Two Buttons: Signup and Login */}
          <div className="hero-buttons">
            <Button variant="primary" size="lg" href="/signup" className="me-3">
              Get Started
            </Button>
            <Button variant="outline-light" size="lg" href="/login">
              Login
            </Button>
          </div>
        </Container>
      </div>

      
    </div>
  );
};

export default Home;
