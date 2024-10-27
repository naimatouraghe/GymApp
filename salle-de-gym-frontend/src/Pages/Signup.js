import React, { useState } from 'react';
import axios from 'axios';
import { Form, Button, Alert, Container, Row, Col } from 'react-bootstrap';

function Signup() {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [phone, setPhone] = useState('');
  const [role] = useState('utilisateur'); // Default role set to 'utilisateur'

  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();

    // Clear previous messages
    setError('');
    setSuccess('');

    // Basic validation
    if (!name || !email || !password) {
      setError('Please fill out all required fields.');
      return;
    }

    const user = {
      nom: name,
      email,
      motDePasse: password,
      telephone: phone,
      role // The default role 'utilisateur' will be sent with the request
    };

    // Make the API request to create a new user
    axios.post('https://localhost:7126/api/Utilisateur', user)
      .then(response => {
        setSuccess('User created successfully!');
        setError('');
        // Optionally reset the form
        setName('');
        setEmail('');
        setPassword('');
        setPhone('');
      })
      .catch(error => {
        setError('Error creating the user. Please try again.');
      });
  };

  return (
    <Container>
      <Row className="justify-content-md-center my-4">
        <Col md={6}>
          <h2 className="text-center">Sign Up</h2>

          {error && <Alert variant="danger">{error}</Alert>}
          {success && <Alert variant="success">{success}</Alert>}

          <Form onSubmit={handleSubmit}>
            <Form.Group controlId="formName" className="mb-3">
              <Form.Label>Name</Form.Label>
              <Form.Control
                type="text"
                placeholder="Enter your name"
                value={name}
                onChange={(e) => setName(e.target.value)}
                required
              />
            </Form.Group>

            <Form.Group controlId="formEmail" className="mb-3">
              <Form.Label>Email</Form.Label>
              <Form.Control
                type="email"
                placeholder="Enter your email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </Form.Group>

            <Form.Group controlId="formPassword" className="mb-3">
              <Form.Label>Password</Form.Label>
              <Form.Control
                type="password"
                placeholder="Enter your password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
            </Form.Group>

            <Form.Group controlId="formPhone" className="mb-3">
              <Form.Label>Phone</Form.Label>
              <Form.Control
                type="text"
                placeholder="Enter your phone number"
                value={phone}
                onChange={(e) => setPhone(e.target.value)}
              />
            </Form.Group>

            {/* Hidden role field */}
            <Form.Control type="hidden" value={role} />

            <Button variant="primary" type="submit" className="w-100">
              Sign Up
            </Button>
          </Form>
        </Col>
      </Row>
    </Container>
  );
}

export default Signup;
