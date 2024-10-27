import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Container, Row, Col, Card, Spinner, Alert, Button } from 'react-bootstrap';
import { useParams, useNavigate } from 'react-router-dom'; // Import useNavigate
import './ActivityDetail.css'; // Import your custom CSS file

const ActivityDetail = () => {
  const { id } = useParams(); // Get the activity ID from the URL
  const [activity, setActivity] = useState(null); // Store the activity details
  const [loading, setLoading] = useState(true); // Loading state
  const [error, setError] = useState(''); // Error message
  const navigate = useNavigate(); // Initialize useNavigate

  // Fetch the activity details from the API
  useEffect(() => {
    axios.get(`https://localhost:7126/api/Activite/${id}`)
      .then(response => {
        setActivity(response.data); // Set the activity details
        setLoading(false); // Set loading to false
      })
      .catch(error => {
        console.error('Error fetching activity details:', error);
        setError('Failed to load activity details.'); // Set error message
        setLoading(false); // Set loading to false
      });
  }, [id]);

  // Handle navigation to reservation page
  const handleBookNow = () => {
    navigate('/reservation'); // Redirect to reservation page
  };

  // Render loading or error state
  if (loading) {
    return <Spinner animation="border" className="spinner" />;
  }

  if (error) {
    return <Alert variant="danger">{error}</Alert>;
  }

  return (
    <Container className="activity-detail-container">
      <Row className="my-4">
        {activity && (
          <>
            <Col md={6}>
              <Card className="activity-card">
                <Card.Img variant="top" src={activity.imageUrl || "placeholder-image-url.jpg"} alt={activity.nom} />
                <Card.Body className="d-flex flex-column justify-content-between" style={{ height: '100%' }}>
                  
                  <div className="mt-auto d-flex  justify-content-between"> {/* Pushes the button to the bottom */}
                  <Card.Title className="activity-title">{activity.nom}</Card.Title>
                    <Button variant="primary" size="lg" onClick={handleBookNow}>Book Now</Button>
                  </div>
                </Card.Body>
              </Card>
            </Col>
            <Col md={6} className="details-section">
              <h4 className="mb-4">Additional Information</h4>
              <p><strong>Capacity:</strong> {activity.capaciteMax}</p>
              <p><strong>Description:</strong> {activity.description}</p>
              <p><strong>Date:</strong> {new Date(activity.dateHeure).toLocaleDateString()}</p>
              <p><strong>Time:</strong> {new Date(activity.dateHeure).toLocaleTimeString()}</p>
            </Col>
          </>
        )}
      </Row>
    </Container>
  );
};

export default ActivityDetail;
