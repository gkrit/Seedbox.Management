Seedbox.Management Application
====================================

Demo application for making team familiar with Containerization (Docker Containers) and Microservices\
Based on transmission Bittorrent Client\
[Seedbox] <https://rollmaps.com/page-238/seedbox-2-2/>

---

## Purpose

Here, in this demo application, we want to aggregate multiple sessions of transmission-daemon that is, aggregate multiple seedboxes under central management

---

## Transmission

Transmission is a Bittorrent client

* <https://transmissionbt.com>
* <https://github.com/transmission>

Transmission-daemon is a daemon-based Transmission session that can be controlled via RPC commands

<https://www.systutorials.com/docs/linux/man/1-transmission-daemon/>

### Transmission architecture

<https://github.com/transmission/transmission/wiki/Transmission-Architecture>

---

## Demo app architecture

Demo application will utilize

* several seedbox nodes for running transmission-daemon bittorrent rpc server. These nodes are the transmission sessions.
* one seedbox manager which acts as client for the seedbox nodes
* one database available for persisting metadata info for each torrent
* web client application

---

## Development Environment

* Windows 10
* Docker Desktop
<https://docs.docker.com/docker-for-windows/?utm_source=docker4win_3.2.2&utm_medium=docs&utm_campaign=referral>
* Visual Studio Code
    Docker Extension
* Powershell
* Windows features enabled - Hyper-V, Containers

---

## Container Types

Available Docker Containers types

* Linux containers in WSL 2 mode
* Linux containers in Hyper-V mode
* Windows containers

---

## Run Application

Here are the steps required to run the application

1. Create a network named seedbox-network

    ```
    docker network create seedbox-network
    ```

2. Find the subnet of the seedbox-network .

    ```
    docker network inspect seedbox-network
    
    {
        .............
        {
            "Subnet": "172.18.0.0/16",
            "Gateway": "172.18.0.1"
        }
        .............
    }
    ```

    We need the subnet in order to whitelist the access to the rpc server.

    ```
    e.g 172.18.*
    ```

3. Specify a TCP/UDP port for each seedbox.
   This is a port of the bittorrent protocol.

   ```
   e.g 51414
   ```

   So, in order to have connectivity with the outside world we must
   make the necessary network settings to open the above port .
   a. in the router, that is, port forwarding to the docker host.
   b. in the docker host firewall

   In this application we have provisioned the following ports

   ```
   51414, 51415, 51416 (3 seedboxes)
   ```

4. If we need direct access from the docker host to each seedbox (through available gui client)
    then we must specify an additional port.

    ```
    eg. 9092
    ```

    In this application we will use an availble windows gui client for direct access to each seedbox container.
    This is not obligatory.
    If this is the case, the following ports are also provisioned :

    ```
    9092, 9093, 9094
    ```

5. We are ready to run a seedbox instance based
on the following linux image zimme/transmission-daemon.

    ><https://github.com/zimme/docker-transmission-daemon>

    It is a minimal image to run the transmission-daemon.exe. Size is only 11MB

6. Run command

    ><https://docs.docker.com/engine/reference/run/>

    When we run a transition-daemon instance we must specify (amomg other things) two mount points with the host machine.

    * config folder mount
  
        This is an initially empty folder.
        When the transition-daemon starts, it generates initial configuration artifacts.

    * downloads folder mount
  
        This is the output of the transmission-daemon

    >we set the working directory at root of the current repository.\
    >**$pwd** for powershell\
    >**%cd%** for command prompt


    ```
    docker run -d --init --name seedbox1 --hostname seedbox1 --network seedbox-network -p 9092:9091 -p 51414:51414 -p 51414:51414/udp -e TZ=Europe/Athens -v $pwd\Transmission\daemon\linux\config\seedbox1:/config -v $pwd\Transmission\daemon\linux\downloads:/var/lib/transmission/Downloads zimme/transmission-daemon
    ```

    ```
    docker run -d --init --name seedbox2 --hostname seedbox2 --network seedbox-network -p 9093:9091 -p 51415:51415 -p 51415:51415/udp -e TZ=Europe/Athens -v $pwd\Transmission\daemon\linux\config\seedbox2:/config -v $pwd\Transmission\daemon\linux\downloads:/var/lib/transmission/Downloads zimme/transmission-daemon
    ```

    ```
    docker run -d --init --name seedbox3 --hostname seedbox3 --network seedbox-network -p 9094:9091 -p 51416:51416 -p 51416:51416/udp -e TZ=Europe/Athens -v $pwd\Transmission\daemon\linux\config\seedbox3:/config -v $pwd\Transmission\daemon\linux\downloads:/var/lib/transmission/Downloads zimme/transmission-daemon

    ```

    ### Error case

    >docker: Error response from daemon: Ports are not available: listen udp 0.0.0.0:51414: bind: An attempt was made to access a socket in a way forbidden by its access permissions.\
    <https://medium.com/@Bartleby/ports-are-not-available-listen-tcp-0-0-0-0-3000-165892441b9d>\
    <https://stackoverflow.com/questions/65272764/ports-are-not-available-listen-tcp-0-0-0-0-50070-bind-an-attempt-was-made-to>

    >netsh interface ipv4 show excludedportrange protocol=tcp\
    >netsh interface ipv4 show excludedportrange protocol=udp

    >net stop winnat\
    >[execute all docker run commands]\
    >net start winnat

7. Stop Containers to edit configuration
    
    ```
    docker stop seedbox1 seedbox2 seedbox3
    ```

8. >Edit config file settings.json
    located in $pwd\Transmission\daemon\linux\config\seedbox{n}
    
    1. Change the following line

        ```
        "rpc-whitelist": "172.18.*"
        ```

    2. Change the following line

        ```
        "rpc-host-whitelist": "seedbox1,seedbox2,seedbox3"
        ```

    3. Change the following line

        ```
        "peer-port": "51413"
        ```
        
        to the port associated to this container 
        
        ```
        e.g 51414
        ```

9.  Start the containers
    
    ```
    docker start seedbox1 seedbox2 seedbox3
    ```
