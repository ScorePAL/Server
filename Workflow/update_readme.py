import requests
import json

swagger_url = "http://localhost:8080/swagger/v1/swagger.json"
readme_path = "../README.md"
start_marker = "## API Routes"

resp = requests.get(swagger_url)
data = resp.json()

lines = [start_marker]
for path, methods in data["paths"].items():
    for method, details in methods.items():

        name = path.split("/")

        if name[1] == "sse" or name[1] == "ws":
            lines.append(f"### {name[1].upper()}")
        else:
            controller = name[2] if len(name) > 2 else ""
            action = name[3] if len(name) > 3 else ""

            lines.append(f"### {controller.capitalize()}-{action.capitalize()}")

        summary = details.get("summary", "")
        lines.append("```http")
        lines.append(f"{method.upper()} {path}")
        lines.append("```")

        params = details.get("parameters", [])
        body = details.get("requestBody", {})
        if params or body:
            lines.append(f"| Parameter | Type     | ")
            lines.append(f"|-----------|----------|")
            for param in details.get("parameters", []):
                param_name = param.get("name", "")
                param_type = param.get("type", "string")
                lines.append(f"| {param_name} | {param_type} |")

        if body:
            content = body.get("content", {})
            for content_type, schema in content.items():
                if "application/json" in content_type:
                    params = schema.get("schema", {})
                    for param_name, param_details in params.items():
                        param_type = param_details.split("/")[-1]
                        lines.append(f"| Body | {param_type} |")


with open(readme_path, "r") as f:
    content = f.read()

start = content.find(start_marker)
new_content = content[:start] + "\n".join(lines)

with open(readme_path, "w") as f:
    f.write(new_content)
