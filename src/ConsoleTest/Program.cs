using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using System.Linq;

namespace src
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(AppContext.BaseDirectory);
            Console.WriteLine("Connecting to Docker...");

            var client = new DockerClientConfiguration(new Uri("unix:///private/var/run/docker.sock"))
                .CreateClient();
            Task.Run(async () => {
                Console.WriteLine("List Containers..");
                var list = await GetContainers(client);
                foreach(var container in list) {
                    Console.WriteLine("ID: {0} Name: {2}. State: ({1}) Created: {3}", 
                    container.ID, container.State, container.Names[0], container.Created);
                }
                Console.WriteLine("Create Container...");
                var colors = new string[]{"yellow", "red", "blue", "green"};
                foreach(var color in colors) {
                    var createdContainer = await CreateContainterIfExists(client, color);
                    Console.WriteLine("ID: {0}", createdContainer.ID);
                    Console.WriteLine("Starting Container...{0}", await StartContainer(client, createdContainer.ID));
                }
            }).Wait();
        }

        static async Task<IList<ContainerListResponse>> GetContainers(DockerClient client) {
            return await client.Containers.ListContainersAsync(
                new ContainersListParameters(){
                    Limit = 50,
                
                });    
        }

        static async Task<CreateContainerResponse> CreateContainterIfExists(DockerClient client, string color) {
            var container = await GetContainerByName(client, color);
            if(container != null)
            {
                Console.WriteLine("Container exists, state: {0}", container.State);
                return new CreateContainerResponse(){
                    ID = container.ID,
                    Warnings = new List<string>{container.State}
                    
                };
            } else {
                return await CreateContainter(client, color);
            }
        }
        static async Task<ContainerListResponse> GetContainerByName(DockerClient client, string color) {
           var name = $"{color}-container.local";
           Console.WriteLine($"Container: {name}");
             var p = new ContainersListParameters();
            p.Filters = new Dictionary<string, IDictionary<string, bool>>();
            p.Filters.Add("label", new Dictionary<string,bool>(){{"name=" + name, true}});
            var result = await client.Containers.ListContainersAsync(p);
            if(result != null && result.Count > 0) {
                return result.FirstOrDefault();
            }
            return null;
        }
        static async Task<CreateContainerResponse> CreateContainter(DockerClient client, string color) {

            var name = $"{color}-container.local";
            var containerParams = new CreateContainerParameters() {
                Name = name,
            };
            containerParams.Labels = new Dictionary<string,string>(){{"name",name}};
            containerParams.Image = "nginx:alpine";
            containerParams.NetworkDisabled = false;
            containerParams.Env = new List<string>{"VIRTUAL_HOST="+ name};
            containerParams.HostConfig = new HostConfig();
            containerParams.HostConfig.NetworkMode = "nginx-proxy";
            containerParams.HostConfig.Mounts = new List<Mount>();
            containerParams.HostConfig.Mounts.Add(new Mount(){
                Source = "/../../../../src/static/" + color,
                Target = "/usr/share/nginx/html",
                ReadOnly = true,
                Type = "bind",
            });
           containerParams.HostConfig.RestartPolicy = new RestartPolicy(){
               Name = RestartPolicyKind.Always
            };

            return await client.Containers.CreateContainerAsync(containerParams);
        }

        static async Task<bool> StartContainer(DockerClient client, string id) {
            return await client.Containers.StartContainerAsync(id, new ContainerStartParameters());
        }
    }
}




/*
    image: nginx:alpine
    container_name: yellow-local
    volumes:
      - ./src/static/yellow:/usr/share/nginx/html:ro
    environment:
      - VIRTUAL_HOST=yellow-container.local




 */
