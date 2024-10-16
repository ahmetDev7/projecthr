import unittest
import httpx 

# 5 test integrations happy path
# 2 test integrations edge cases
class TestGlobal(unittest.TestCase):
    
    def setUp(self):        
        self.admin_token = "a1b2c3d4e5"
        self.reader_token = "f6g7h8i9j0"
        self.headers = {"Content-Type": "application/json", "API_KEY": ""}
        self.client = httpx.Client(base_url="http://localhost:3000/api/v1", headers=self.headers)    

    def set_admin_api_key(self):
        self.client.headers["API_KEY"] = self.admin_token
        
    def set_outsider_api_key(self):
        self.client.headers["API_KEY"] = "invalid-key"

    def test_get_all_as_admin(self):
        self.set_admin_api_key()
        endpoint = f"{self.client.base_url}locations"
        response = self.client.get(url=endpoint)
        self.assertEqual(response.status_code, httpx.codes.OK)

    def test_get_single_as_admin(self):
        self.set_admin_api_key()
        endpoint = f"{self.client.base_url}warehouses/1"
        response = self.client.get(url=endpoint)
        self.assertEqual(True, response.status_code == httpx.codes.OK)

    def test_get_single_outsider(self):
        self.set_outsider_api_key()

        endpoint = f"{self.client.base_url}locations/1"
        response = self.client.get(url=endpoint)
        self.assertEqual(True, response.status_code == httpx.codes.UNAUTHORIZED)

    def test_get_all_outsider(self):
        self.set_outsider_api_key()
        endpoint = f"{self.client.base_url}shipments/1"
        response = self.client.get(url=endpoint)
        self.assertEqual(True, response.status_code == httpx.codes.UNAUTHORIZED)

    def test_create_as_admin(self):
        self.set_admin_api_key()
        body = {
            "id": 1,
            "warehouse_id": 1,
            "code": "unit test",
            "name": "unit test test",
            "created_at": "2024-08-05 16:21:20",
            "updated_at": "2024-08-05 16:21:20",
        }

        response = self.client.post(url=f"{self.client.base_url}locations", json=body)
        self.assertEqual(True, response.status_code == httpx.codes.CREATED)

    def test_update_as_admin(self):
        self.set_admin_api_key()

        body = {
            "id": 1,
            "name": "sidi cha",
            "address": "Dahlgata 4",
            "city": "Øysteinnes",
            "zip_code": "8817",
            "country": "Norway",
            "contact_name": "Anette Næss",
            "contact_phone": "65319533",
            "contact_email": "grosaether@example.com",
            "created_at": "2012-12-06 05:59:02",
            "updated_at": "2015-08-18 23:46:10",
        }

        response = self.client.put(url=f"{self.client.base_url}clients/1", json=body)
        self.assertEqual(True, response.status_code == httpx.codes.OK)
        
    def test_delete_as_admin(self):
        self.set_admin_api_key()
        response = self.client.delete(url=f"{self.client.base_url}orders/1")
        self.assertEqual(True, response.status_code == httpx.codes.OK)


if __name__ == "__main__":
    unittest.main()